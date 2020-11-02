import cv2
import grpc
import sys
import os
import numpy as np
import time

import icp_pb2
import icp_pb2_grpc


grpc_options = [
        ("grpc.max_message_length", 3840 * 2160 * 32),
        ("grpc.max_send_message_length", 3840 * 2160 * 32),
        ("grpc.max_receive_message_length", 3840 * 2160 * 32)
    ]

channel = grpc.insecure_channel("localhost:50051", options=grpc_options)

stub = icp_pb2_grpc.CalcTransformStub(channel)


def clouds2msg(A, B):
    A_cloud = []
    for row in A:
        A_cloud.append(icp_pb2.Point(x=row[0], y=row[1], z=row[2]))
    B_cloud = []
    for row in B:
        B_cloud.append(icp_pb2.Point(x=row[0], y=row[1], z=row[2]))
    A_cloud = icp_pb2.Cloud(points=A_cloud)
    B_cloud = icp_pb2.Cloud(points=B_cloud)
    return icp_pb2.ObjectClouds(clouds=[B_cloud, A_cloud])

def rotation_matrix(axis, theta):
    axis = axis/np.sqrt(np.dot(axis, axis))
    a = np.cos(theta/2.)
    b, c, d = -axis*np.sin(theta/2.)

    return np.array([[a*a+b*b-c*c-d*d, 2*(b*c-a*d), 2*(b*d+a*c)],
                  [2*(b*c+a*d), a*a+c*c-b*b-d*d, 2*(c*d-a*b)],
                  [2*(b*d-a*c), 2*(c*d+a*b), a*a+d*d-b*b-c*c]])

def test_icp():
    N = 200                                 
    dim = 3 
    # Generate a random dataset
    A = np.random.rand(N, dim)

    total_time = 0

    B = np.copy(A)

    # Translate
    t = np.random.rand(dim)*1
    B += t

    # Rotate
    R = rotation_matrix(np.random.rand(dim), np.random.rand() * 1)
    B = np.dot(R, B.T).T

    # # Add noise
    # B += np.random.randn(N, dim) * noise_sigma

    # Shuffle to disrupt correspondence
    np.random.shuffle(B)

    # Run ICP
    start = time.time()

    msg = clouds2msg(B, A)
    res = stub.getTransform(msg)
    T = res.vals
    T = np.array(T).reshape(4,4)
    print(T)

    total_time += time.time() - start

    # # Make C a homogeneous representation of B
    # C = np.ones((N, 4))
    # C[:,0:3] = np.copy(B)

    # # Transform C
    # C = np.dot(T, C.T).T

    # # assert np.mean(distances) < 6*noise_sigma                   # mean error should be small
    # # assert np.allclose(T[0:3,0:3].T, R, atol=6*noise_sigma)     # T and R should be inverses
    # # assert np.allclose(-T[0:3,3], t, atol=6*noise_sigma)        # T and t should be inverses

    print(t)
    print(R.T)
    print('icp time: {:.3}'.format(total_time))

    return


if __name__ == "__main__":
    test_icp()