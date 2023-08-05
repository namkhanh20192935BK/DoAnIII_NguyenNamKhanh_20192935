import cv2
import numpy as np
from skimage import measure
from imutils import perspective
import imutils

from src.data_utils import order_points, convert2Square, draw_labels_and_boxes
from src.lp_detection.detect import detectNumberPlate
from src.char_classification.model import CNN_Model
from skimage.filters import threshold_local

ALPHA_DICT = {0: 'A', 1: 'B', 2: 'C', 3: 'D', 4: 'E', 5: 'F', 6: 'G', 7: 'H', 8: 'K', 9: 'L', 10: 'M', 11: 'N', 12: 'P',
              13: 'R', 14: 'S', 15: 'T', 16: 'U', 17: 'V', 18: 'X', 19: 'Y', 20: 'Z', 21: '0', 22: '1', 23: '2',
              24: '3', 25: '4', 26: '5', 27: '6', 28: '7', 29: '8', 30: '9', 31: "Background"}

LP_DETECTION_CFG = {
    "weight_path": "./src/weights/yolov3-tiny_15000.weights",
    "classes_path": "./src/lp_detection/cfg/yolo.names",
    "config_path": "./src/lp_detection/cfg/yolov3-tiny.cfg"
}

CHAR_CLASSIFICATION_WEIGHTS = './src/weights/weight.h5'


class E2E(object):
    def __init__(self):
        #Khởi tạo mảng 1 chiều rỗng để lưu hình ảnh
        self.image = np.empty((28, 28, 1))
        #Tạo một đối tượng detectNumberPlate (được khởi tạo từ một số đường dẫn cụ thể) để phát hiện vùng chứa biển số xe.
        self.detectLP = detectNumberPlate(LP_DETECTION_CFG['classes_path'], LP_DETECTION_CFG['config_path'],
                                          LP_DETECTION_CFG['weight_path'])
        #Tạo một đối tượng CNN_Model để nhận dạng các ký tự trong biển số xe. Đối tượng này được khởi tạo với tham số trainable=False để ngăn việc cập nhật trọng số trong quá trình huấn luyện.
        self.recogChar = CNN_Model(trainable=False).model
        #Nạp trọng số cho mô hình nhận dạng ký tự từ đường dẫn CHAR_CLASSIFICATION_WEIGHTS.
        self.recogChar.load_weights(CHAR_CLASSIFICATION_WEIGHTS)
        #Khởi tạo một danh sách trống để lưu trữ các ứng cử viên (candidates) cho biển số xe.
        self.candidates = []

    def extractLP(self):
        # Sử dụng detectLP.detect(self.image) để tìm kiếm vị trí của biển số xe trong hình ảnh. Kết quả trả về là một
        # danh sách các tọa độ (coordinates) tương ứng với vị trí của các biển số xe được phát hiện.
        coordinates = self.detectLP.detect(self.image)
        # Kiểm tra xem có biển số xe được phát hiện hay không. Nếu danh sách coordinates rỗng, hàm sẽ raise một
        # ValueError thông báo là "No images detected".
        if len(coordinates) == 0:
            ValueError('No images detected')

        #Nếu có biển số xe được phát hiện, hàm sẽ sử dụng vòng lặp for để duyệt qua từng tọa độ (coordinate) trong danh sách coordinates.
        #Hàm sẽ yield (trả về) mỗi tọa độ coordinate trong vòng lặp. Việc sử dụng yield cho phép hàm trở thành một generator, cho phép lặp qua các tọa độ một cách linh hoạt.
        for coordinate in coordinates:
            yield coordinate

    def predict(self, image):
        # Input image or frame
        #Gán hình ảnh đầu vào cho thuộc tính self.image.
        self.image = image

        # detect license plate by yolov3
        # Phát hiện biển số sử dụng Yolov3
        # Sử dụng vòng lặp for để duyệt qua từng tọa độ (coordinate) được trích xuất từ hình ảnh sử dụng hàm self.extractLP().
        for coordinate in self.extractLP():
            #Trong mỗi vòng lặp, đặt danh sách self.candidates thành rỗng.
            self.candidates = []

            # convert (x_min, y_min, width, height) to coordinate(top left, top right, bottom left, bottom right)
            #Sử dụng hàm order_points(coordinate) để chuyển đổi tọa độ (x_min, y_min, width, height) thành tọa độ (top left, top right, bottom left, bottom right).
            pts = order_points(coordinate)

            # Thực hiện cắt xén (crop) vùng chứa biển số xe từ hình ảnh sử dụng phép biến đổi "bird's eye view" (chế độ nhìn từ trên cao) dựa trên các điểm tọa độ đã tính toán trước đó.
            # crop number plate used by bird's eyes view transformation
            LpRegion = perspective.four_point_transform(self.image, pts)

            # segmentation
            # Tiến hành phân đoạn (segmentation) trên vùng biển số xe đã cắt xén.
            self.segmentation(LpRegion)

            # recognize characters
            # Nhận dạng các ký tự trong biển số xe sử dụng hàm self.recognizeChar()
            self.recognizeChar()

            # format and display license plate
            # Định dạng và hiển thị biển số xe.
            license_plate = self.format()

            # draw labels
            # Vẽ nhãn (label) và hộp (box) trên hình ảnh gốc để chỉ ra vị trí của biển số xe.
            self.image = draw_labels_and_boxes(self.image, license_plate, coordinate)
            # print("Biển số xe là: ",license_plate)

        #Trả về hình ảnh đã được xử lý.
        #return self.image
        # Test trả về biển số
        return license_plate

    def segmentation(self, LpRegion):
        # apply thresh to extracted licences plate
        # Chia kênh V (Value) từ không gian màu HSV của hình ảnh biển số xe.
        V = cv2.split(cv2.cvtColor(LpRegion, cv2.COLOR_BGR2HSV))[2]

        # Áp dụng ngưỡng (thresholding) cho kênh V bằng phương pháp ngưỡng thích ứng (adaptive thresholding). Kết quả
        # của bước này là một hình ảnh nhị phân, trong đó các pixel được phân loại là đen hoặc trắng.
        # adaptive threshold
        T = threshold_local(V, 15, offset=10, method="gaussian")
        thresh = (V > T).astype("uint8") * 255

        cv2.imshow("Ảnh đưa về mức xám", thresh)

        # Đảo ngược hình ảnh nhị phân để chuyển các pixel đen thành pixel trắng và ngược lại.
        # convert black pixel of digits to white pixel
        thresh = cv2.bitwise_not(thresh)
        # Thay đổi kích thước hình ảnh nhị phân thành chiều rộng 400 pixel (với tỷ lệ giữ nguyên).
        thresh = imutils.resize(thresh, width=400)
        # Làm mờ hình ảnh nhị phân bằng bộ lọc trung vị (median blur) với kích thước kernel là 5x5.
        thresh = cv2.medianBlur(thresh, 5)

        # Phân tích thành phần kết nối (connected components analysis) trên hình ảnh nhị phân để tách các thành phần
        # riêng biệt trong biển số xe.
        # connected components analysis
        labels = measure.label(thresh, connectivity=2, background=0)

        # Vòng lặp qua các thành phần duy nhất trong hình ảnh.
        # loop over the unique components
        for label in np.unique(labels):
            # Nếu nhãn hiện tại là nhãn nền (background), bỏ qua và tiếp tục với nhãn khác.
            # if this is background label, ignore it
            if label == 0:
                continue

            # Khởi tạo mặt nạ (mask) để lưu trữ vị trí các ứng viên ký tự.
            # init mask to store the location of the character candidates
            mask = np.zeros(thresh.shape, dtype="uint8")
            mask[labels == label] = 255

            # Tìm các đường viền (contours) từ mặt nạ.
            # find contours from mask
            contours, hierarchy = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

            #Nếu tìm thấy ít nhất một đường viền
            if len(contours) > 0:
                #Chọn đường viền có diện tích lớn nhất làm đường viền chính (contour).
                contour = max(contours, key=cv2.contourArea)
                (x, y, w, h) = cv2.boundingRect(contour)

                # Xác định các quy tắc để xác định các ký tự trong đường viền chính, bao gồm tỷ lệ khung hình, độ kết dính và tỷ lệ chiều cao.
                # rule to determine characters
                aspectRatio = w / float(h)
                solidity = cv2.contourArea(contour) / float(w * h)
                heightRatio = h / float(LpRegion.shape[0])

                # Nếu các quy tắc được đáp ứng, trích xuất ứng viên ký tự từ hình ảnh và thêm chúng vào danh sách các ứng viên ký tự (self.candidates).
                if 0.1 < aspectRatio < 1.0 and solidity > 0.1 and 0.35 < heightRatio < 2.0:
                    # extract characters
                    candidate = np.array(mask[y:y + h, x:x + w])
                    square_candidate = convert2Square(candidate)
                    square_candidate = cv2.resize(square_candidate, (28, 28), cv2.INTER_AREA)
                    square_candidate = square_candidate.reshape((28, 28, 1))
                    cv2.imshow("image", square_candidate)
                    self.candidates.append((square_candidate, (y, x)))

    def recognizeChar(self):
        characters = []
        coordinates = []

        for char, coordinate in self.candidates:
            characters.append(char)
            coordinates.append(coordinate)

        characters = np.array(characters)
        result = self.recogChar.predict_on_batch(characters)
        result_idx = np.argmax(result, axis=1)

        self.candidates = []
        for i in range(len(result_idx)):
            if result_idx[i] == 31:  # if is background or noise, ignore it
                continue
            self.candidates.append((ALPHA_DICT[result_idx[i]], coordinates[i]))

    # Xác định biển số 1 dòng hoặc 2 dòng
    def format(self):
        # Dòng đầu
        first_line = []
        # Dòng sau
        second_line = []

        for candidate, coordinate in self.candidates:
            # self.candidates[0][1][0] lấy ra tọa độ x của dữ liệu đầu tiên và self.candidates[0][1][0] + 40 là giá
            # trị tọa độ x tăng thêm 40 so với tọa độ ban đầu coordinate[0] đại diện cho tọa độ x của giá trị trong
            # vòng lặp hiện tại
            if self.candidates[0][1][0] + 40 > coordinate[0]:
                first_line.append((candidate, coordinate[1]))
            else:
                second_line.append((candidate, coordinate[1]))

        #Thuật toán sắp xếp dựa trên phần tử thứ 2, theo tọa độ y
        def take_second(s):
            return s[1]

        first_line = sorted(first_line, key=take_second)
        second_line = sorted(second_line, key=take_second)

        # In biển số xe thành chữ và đưa vào license plate
        if len(second_line) == 0:  # Nếu là biển số 1 dòng
            if(len(first_line)==8):
                license_plate = "".join([str(ele[0]) for ele in first_line[0:3]]) + "-" + "".join(
                    [str(ele[0]) for ele in first_line[3:len(first_line)-2]]) + "." + "".join(
                    [str(ele[0]) for ele in first_line[len(first_line)-2:]])
            else:
                license_plate = "".join([str(ele[0]) for ele in first_line[0:3]]) + "-" + "".join(
                    [str(ele[0]) for ele in first_line[3:]])

        else:
            if(len(second_line) == 5):  # Nếu là biển số 2 dòng
                license_plate = "".join([str(ele[0]) for ele in first_line]) + "-" + "".join(
                    [str(ele[0]) for ele in second_line[:len(second_line)-2]]) + "." + "".join([str(ele[0]) for ele in second_line[len(second_line)-2:]])
            else:
                license_plate = "".join([str(ele[0]) for ele in first_line]) + "-" + "".join(
                    [str(ele[0]) for ele in second_line])

        return license_plate
