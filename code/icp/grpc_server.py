import cv2
import numpy as np
import sys
import time
import os
import pathlib
from ctypes import *

from concurrent import futures
import grpc

import icp_pb2
import icp_pb2_grpc

import icp

class CalcTransformServicer(icp_pb2_grpc.CalcTransformServicer):
    def getTransform(self, request, context):
        clouds = request.clouds
        A = clouds[1]
        B = clouds[0]
        vals = []
        A, B = self.msg2np(A, B)
        print(A)
        T, distances, iterations = icp.icp(B, A, tolerance=0.000001)
        msg = self.info2msg(T)
        return msg
    
    def info2msg(self, T):
        T = T.flatten()
        vals = []
        for val in T:
            vals.append(val)
        msg = icp_pb2.Transform(vals=vals)
        return msg

    def msg2np(self, A, B):
        array = []
        for point in A.points:
            array.append(point.x)
            array.append(point.y)
            array.append(point.z)
        A = np.array(array).reshape(-1,3)
        array = []
        for point in B.points:
            array.append(point.x)
            array.append(point.y)
            array.append(point.z)
        B = np.array(array).reshape(-1,3)
        return A, B

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
    icp_pb2_grpc.add_CalcTransformServicer_to_server(CalcTransformServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    serve()
