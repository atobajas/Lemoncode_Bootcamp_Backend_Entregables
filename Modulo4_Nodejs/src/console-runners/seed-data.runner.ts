import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';
import { generateSalt, hashPassword } from 'common/helpers';
import { userRepository } from 'dals';
import { db } from 'dals/mock-data';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);

  for (const user of db.users) {
    const salt = await generateSalt();
    const hashedPassword = await hashPassword(user.password, salt);
    await userRepository.saveUser({
      ...user,
      password: hashedPassword,
      salt,
    });
  }
};
