import { User } from '../user.model';

export interface UserRepository {
  getUserList: () => Promise<User[]>;
  getUserByEmailAndPassword: (email: string, password: string) => Promise<User>;
  saveUser: (user: User) => Promise<Boolean>;
  deleteUser: (email: string) => Promise<boolean>;
}
