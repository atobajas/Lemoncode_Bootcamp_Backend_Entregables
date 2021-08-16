import { House } from '.';
import { ObjectId } from 'mongodb';

export interface DB {
  houses: House[];
}

export const db: DB = {
  houses: [
    {
      _id: '10009990',
      name: 'Casa Rural Tereñes',
      listing_url: 'www.casa1.es',
      description: 'Descripción casa Tereñes',
      room_type: 'Doble',
      bed_type: 'Matrimonio',
      first_review: new Date(),
      last_review: new Date(),
      beds: 1,
      bathrooms: 1,
      address: 'España',
    },
    {
      _id: '10009991',
      name: 'Casa Rural La Cueste',
      listing_url: 'www.casa2.es',
      description: 'Descripción casa La Cueste',
      room_type: 'Doble',
      bed_type: 'Matrimonio',
      first_review: new Date(),
      last_review: new Date(),
      beds: 2,
      bathrooms: 1,
      address: 'España',
    },
  ],
};
