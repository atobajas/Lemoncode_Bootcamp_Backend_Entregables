import { House } from '.';

export interface DB {
  houses: House[];
}

export const db: DB = {
  houses: [
    {
      _id: '1',
      name: 'Casa Rural Tereñes',
    },
    {
      _id: '2',
      name: 'Casa Rural La Cueste',
    },
  ],
};
