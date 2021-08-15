import express from 'express';
import path from 'path';
import {
  logErrorRequestMiddleware,
  logRequestMiddleware,
} from 'common/middleware';
import { createRestApi } from 'core/servers';
import { envConstants } from 'core/constants';
import { housesApi } from 'pods';

const restApiServer = createRestApi();

restApiServer.use(
  '/',
  express.static(path.resolve(__dirname, envConstants.STATIC_FILES_PATH))
);

restApiServer.use(logRequestMiddleware);

restApiServer.use('/api/houses', housesApi);

// Middleware manejo errores siempre el último.
restApiServer.use(logErrorRequestMiddleware);

restApiServer.listen(envConstants.PORT, () => {
  console.log('Server ready at port 3000');
});
