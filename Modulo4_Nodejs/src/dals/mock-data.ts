import { House } from '.';
import { ObjectId } from 'mongodb';

export interface DB {
  houses: House[];
}

export const db: DB = {
  houses: [
    {
      _id: new ObjectId(),
      name: 'Casa Rural Tereñes',
    },
    {
      _id: new ObjectId(),
      name: 'Casa Rural La Cueste',
    },
  ],
};
