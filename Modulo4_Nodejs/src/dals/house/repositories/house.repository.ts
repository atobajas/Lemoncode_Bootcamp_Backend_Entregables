import { House, Review } from '../house.model';

export interface HouseRepository {
  getHouseList: () => Promise<House[]>;
  getHouse: (id: string) => Promise<House>;
  saveHouse: (house: House) => Promise<House>;
  deleteHouse: (id: string) => Promise<boolean>;
  insertHouseReview: (id: string, review: Review) => Promise<House>;
}
