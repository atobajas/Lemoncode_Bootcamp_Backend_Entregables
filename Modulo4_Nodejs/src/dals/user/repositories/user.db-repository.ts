import { hashPassword } from 'common/helpers';
import { getDBInstance } from 'core/servers';
import { User } from 'dals';
import { UserRepository } from './user.repository';

export const dbRepository: UserRepository = {
  getUserList: async () => {
    const db = getDBInstance();
    return await db.collection<User>('users').find().toArray();
  },
  getUserByEmailAndPassword: async (email: string, password: string) => {
    const db = getDBInstance();
    const user = await db.collection<User>('users').findOne({ email: email });
    const hashedPassword = await hashPassword(password, user.salt);
    return user.password === hashedPassword
      ? ({ _id: user._id, email: user.email, role: user.role } as User)
      : null;
  },
  saveUser: async (user: User) => {
    const db = getDBInstance();
    await db.collection<User>('users').findOneAndUpdate(
      {
        _id: user._id,
      },
      {
        $set: user,
      },
      {
        upsert: true,
        returnDocument: 'after',
      }
    );
    return true;
  },
  deleteUser: async (email: string): Promise<boolean> => {
    const db = getDBInstance();
    const { deletedCount } = await db
      .collection<User>('users')
      .deleteOne({ email: email });
    return deletedCount === 1;
  },
};
