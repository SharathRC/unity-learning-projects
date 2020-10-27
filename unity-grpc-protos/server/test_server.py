import cv2
import numpy as np
import sys
import time
import os
import pathlib
from ctypes import *

from concurrent import futures
import grpc
# sys.path.append(os.path.join('../../', 'api'))
import msg_pb2
import msg_pb2_grpc


class DetectorServicer(msg_pb2_grpc.DetectorServicer):
    def detect_img(self, request, context):
        np_img = np.frombuffer(request.data, np.uint8)
        cv_img = np_img.reshape(request.height, request.width, 3)

        objects = []
        new_obj = msg_pb2.Object(
            label='test',
            probability=0.9,
            xmin=0,
            ymin=0,
            xmax=0,
            ymax=0,
        )            
        objects.append(new_obj)
        return msg_pb2.Detection(objects=objects)

class TesterServicer(msg_pb2_grpc.TesterServicer):
    def test_grpc(self, request, context):
        print(context)
        return msg_pb2.TestMsg(x=27)


def serve():
    grpc_options = [
        ("grpc.max_message_length", 3840*2160*32),
        ("grpc.max_send_message_length", 3840 * 2160 * 32),
        ("grpc.max_receive_message_length", 3840 * 2160 * 32)
    ]
    server = grpc.server(
        futures.ThreadPoolExecutor(max_workers=1),
        options=grpc_options
    )
    # server = grpc.server(futures.ThreadPoolExecutor(max_workers=1))
    msg_pb2_grpc.add_DetectorServicer_to_server(DetectorServicer(), server)
    msg_pb2_grpc.add_TesterServicer_to_server(TesterServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    serve()