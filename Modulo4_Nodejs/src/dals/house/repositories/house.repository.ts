import { House } from '../house.model';
import { Review } from '../review.model';

export interface HouseRepository {
  getHouseList: () => Promise<House[]>;
  getHouse: (id: string) => Promise<House>;
  saveHouse: (house: House) => Promise<House>;
  deleteHouse: (id: string) => Promise<boolean>;
<<<<<<< HEAD
  insertHouseReview: (id: string, review: Review) => Promise<House>;
=======
  insertReview: (id: string, review: Review) => Promise<House>;
>>>>>>> 86983236bbbcf54e8f489c158b69206a3398046f
}
