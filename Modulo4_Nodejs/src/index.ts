import express from 'express';
import path from 'path';
import { createRestApi } from './core/servers';
import { envConstants } from './core/constants';

const restApiServer = createRestApi();

restApiServer.use('/', express.static(path.resolve(__dirname, envConstants.STATIC_FILES_PATH)));

// Middleware manejo errores siempre el último.
restApiServer.use(async (error, req, res, next) => {
  console.error(error);
  res.sendStatus(500);
});

restApiServer.listen(envConstants.PORT, () => {
  console.log('Server ready at port 3000');
});
