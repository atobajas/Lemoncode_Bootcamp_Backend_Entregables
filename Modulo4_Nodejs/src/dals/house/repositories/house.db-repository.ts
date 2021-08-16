import { House } from '..';
import { HouseRepository } from './house.repository';
import { getDBInstance } from 'core/servers';

export const dbRepository: HouseRepository = {
  getHouseList: async () => {
    throw new Error('Not implemented');
  },
  getHouse: async (id: string) => {
    throw new Error('Not implemented');
  },
  saveHouse: async (house: House) => {
    throw new Error('Not implemented');
  },
  deleteHouse: async (id: string) => {
    throw new Error('Not implemented');
  },
};
