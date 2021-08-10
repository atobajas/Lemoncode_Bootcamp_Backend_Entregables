import express from 'express';
import cors from 'cors';
import { envConstants } from '../constants';

export const createRestApi = () => {
  const createRestApi = express();
  createRestApi.use(express.json());
  createRestApi.use(
    cors({
      methods: envConstants.CORS_METHODS,
      origin: envConstants.CORS_ORIGIN,
      credentials: true,
    })
  );

  return createRestApi;
};
