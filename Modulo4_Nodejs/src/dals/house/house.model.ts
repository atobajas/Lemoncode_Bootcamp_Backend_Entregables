import { ObjectId } from 'mongodb';

export interface House {
  _id: ObjectId;
  name: string;
}
