import cv2
import numpy as np
import sys
import time
import os
import pathlib
from ctypes import *

from concurrent import futures
import grpc
import color_pb2
import color_pb2_grpc

class ColorGeneratorServicer(color_pb2_grpc.ColorGeneratorServicer):
    def GetRandomColor(self, request, context):
        print(context)
        return color_pb2.NewColor(color='newcolor')


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
    color_pb2_grpc.add_ColorGeneratorServicer_to_server(ColorGeneratorServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    serve()
