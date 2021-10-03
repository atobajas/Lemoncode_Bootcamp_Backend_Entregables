import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';
import { generateSalt, hashPassword } from 'common/helpers';
import { userRepository } from 'dals';
import { db } from 'dals/mock-data';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);

  const salt = await generateSalt();
  const hashedPassword = await hashPassword('password', salt);

  userRepository.saveUser({
    ...db.users[0],
    password: hashedPassword,
    salt,
  });
};
