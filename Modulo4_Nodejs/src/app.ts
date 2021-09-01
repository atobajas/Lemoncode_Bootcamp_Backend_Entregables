import express from 'express';
import path from 'path';
import {
  logErrorRequestMiddleware,
  logRequestMiddleware,
} from 'common/middleware';
import { connectToDBServer, createRestApi } from 'core/servers';
import { envConstants } from 'core/constants';
import { housesApi } from 'pods';
import { securityApi, authenticationMiddleware } from 'pods/security';

const restApiServer = createRestApi();

restApiServer.use(
  '/',
  express.static(path.resolve(__dirname, envConstants.STATIC_FILES_PATH))
);

restApiServer.use(logRequestMiddleware);

restApiServer.use('/api/security', securityApi);
restApiServer.use('/api/houses', authenticationMiddleware, housesApi);

// Middleware manejo errores siempre el último.
restApiServer.use(logErrorRequestMiddleware);

restApiServer.listen(envConstants.PORT, async () => {
  try {
    if (!envConstants.isApiMock) {
      await connectToDBServer(envConstants.MONGODB_URI);
      console.log('Connect to DB');
    } else {
      console.log('Running API mock');
    }
  } catch (error) {
    console.error(error);
  }
  console.log(`Server ready at port ${envConstants.PORT}`);
});
