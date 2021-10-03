import { ObjectId } from 'bson';
import * as model from 'dals/user/user.model';
import * as apiModel from './user.api-model';

export const mapUserFromModelToApi = (user: model.User): apiModel.User => ({
  email: user.email,
  password: user.password,
  role: user.role,
});

export const mapUserFromApiToModel = (user: apiModel.User): model.User => ({
  _id: new ObjectId(),
  email: user.email,
  password: user.password,
  salt: '',
  role: user.role,
});
