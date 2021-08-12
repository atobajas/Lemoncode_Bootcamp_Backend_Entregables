import { mockRepository } from './house.mock-repository';
import { dbRepository } from './house.db-repository';
import { envConstants } from 'core/constants';

export const houseRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;
