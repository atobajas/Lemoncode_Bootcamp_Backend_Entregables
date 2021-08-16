import { House } from '../house.model';

export interface HouseRepository {
  getHouseList: () => Promise<House[]>;
  getHouse: (id: string) => Promise<House>;
  saveHouse: (house: House) => Promise<House>;
  deleteHouse: (id: string) => Promise<boolean>;
}
