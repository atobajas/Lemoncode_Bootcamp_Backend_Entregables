import { mockRepository } from './house.mock-repository';
import { dbRepository } from './house.db-repository';

const isApiMock = true;
export const houseRepository = isApiMock ? mockRepository : dbRepository;
