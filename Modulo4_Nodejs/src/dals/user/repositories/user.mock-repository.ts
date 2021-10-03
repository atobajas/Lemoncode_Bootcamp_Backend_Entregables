import { UserRepository } from './user.repository';
import { db } from '../../mock-data';

export const mockRepository: UserRepository = {
  getUserList: async () => db.users,
  getUserByEmailAndPassword: async (email: string, password: string) =>
    db.users.find((u) => u.email === email && u.password === password),
  saveUser: async () => true,
  deleteUser: async function (email: string): Promise<boolean> {
    db.users.filter((u) => u.email !== email);
    return true;
  },
};
