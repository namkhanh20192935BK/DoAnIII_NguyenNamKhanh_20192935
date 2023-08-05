import os
import cv2
from pathlib import Path
import argparse
import time


from src.lp_recognition import E2E

os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

img_path = "C:/Users/Admin/Downloads/License-Plate-Recognition-master/samples"

def get_arguments():
    arg = argparse.ArgumentParser()
    arg.add_argument('-i', '--image_path', help='link to image', default='./samples/BienVuong_XeMay_Nhaxe_08.jpg')

    return arg.parse_args()
# def get_arguments(image_link):
#     arg = argparse.ArgumentParser()
#     arg.add_argument('-i', '--image_path', help='link to image', default=str(image_link))
#
#     return arg.parse_args()
# image_link = str(input())
args = get_arguments()

img_path = Path(args.image_path)

# read image
img = cv2.imread(str(img_path))

# start
start = time.time()

# load model
model = E2E()

# recognize license plate
image = model.predict(img)

# end
end = time.time()

print('Model process on %.2f s' % (end - start))

# show image
cv2.imshow('License Plate', image)
if cv2.waitKey(0) & 0xFF == ord('q'):
    exit(0)

cv2.destroyAllWindows()
