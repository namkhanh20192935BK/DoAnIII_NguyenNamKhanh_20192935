import os

from flask import Flask
from flask import request, jsonify
from flask_cors import CORS, cross_origin
import numpy as np
import cv2
import base64
from pathlib import Path
import argparse
import time
from src.lp_recognition import E2E

os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

# Khai bao cong cua server
my_port = '8000'
scale = 0.00392
conf_threshold = 0.5
nms_threshold = 0.4

# Doan ma khoi tao server
app = Flask(__name__)
CORS(app)


# Cac ham ho tro chay YOLO

def get_output_layers(net):
    layer_names = net.getLayerNames()
    output_layers = [layer_names[i - 1] for i in net.getUnconnectedOutLayers()]
    return output_layers


def build_return(class_id, x, y, x_plus_w, y_plus_h, name_license_plate):
    return str(class_id) + "," + str(x) + "," + str(y) + "," + str(x_plus_w) + "," + str(y_plus_h) + "," + str(
        name_license_plate)


# Khoi tao model YOLO
# net = cv2.dnn.readNet("yolov3.weights", "yolov3.cfg")
net = cv2.dnn.readNet("src/weights/yolov3-tiny_15000.weights", "src/lp_detection/cfg/yolov3-tiny.cfg")


# Khai bao ham xu ly request detect
@app.route('/detect', methods=['POST'])
@cross_origin()
def detect():
    # Lay du lieu image B64 gui len va chuyen thanh image
    image_b64 = request.form.get('image')
    # image = np.fromstring(base64.b64decode(image_b64), dtype=np.uint8)
    image = np.frombuffer(base64.b64decode(image_b64), dtype=np.uint8)
    image = cv2.imdecode(image, cv2.IMREAD_ANYCOLOR)

    # Lay kich thuoc anh gui len
    Width = image.shape[1]
    Height = image.shape[0]

    # Nhan dien bang YOLO   

    blob = cv2.dnn.blobFromImage(image, scale, (416, 416), (0, 0, 0), True, crop=False)
    net.setInput(blob)
    outs = net.forward(get_output_layers(net))

    class_ids = []
    confidences = []
    boxes = []

    for out in outs:
        for detection in out:
            scores = detection[5:]
            class_id = np.argmax(scores)
            confidence = scores[class_id]
            if confidence > conf_threshold:
                center_x = int(detection[0] * Width)
                center_y = int(detection[1] * Height)
                w = int(detection[2] * Width)
                h = int(detection[3] * Height)
                x = center_x - w / 2
                y = center_y - h / 2
                class_ids.append(class_id)
                confidences.append(float(confidence))
                boxes.append([x, y, w, h])

    indices = cv2.dnn.NMSBoxes(boxes, confidences, conf_threshold, nms_threshold)
    retString = ""

    # start
    start = time.time()

    # load model
    model = E2E()

    # recognize license plate
    name_license_plate = model.predict(image)

    # end
    end = time.time()

    for i in indices:
        box = boxes[i]
        x = box[0]
        y = box[1]
        w = box[2]
        h = box[3]
        # Xay dung chuoi tra ve client
        retString += build_return(class_ids[i], round(x), round(y), round(w), round(h), name_license_plate) + "|"

    return retString;


# Thuc thi server
if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=my_port)
