import { prompt } from 'inquirer';
import { runCommand } from './console-runners.helpers';
import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';
import { generateSalt, hashPassword } from 'common/helpers';
import { userRepository } from 'dals';
import { db } from 'dals/mock-data';

const seedDataContainerPath = '/opt/app';

export const run = async () => {
  // Restore Database
  const { seedDataPath, containerName, dbName } = await prompt([
    {
      name: 'seedDataPath',
      type: 'input',
      message: 'Seed data path (in your file system):',
    },
    {
      name: 'containerName',
      type: 'input',
      message: 'Docker container name:',
    },
    {
      name: 'dbName',
      type: 'input',
      message: 'Database name:',
    },
  ]);

  const copySeedDataCommand = `docker cp "${seedDataPath}" ${containerName}:${seedDataContainerPath}`;
  const restoreBackupCommand = `docker exec ${containerName} mongorestore --db ${dbName} ${seedDataContainerPath}`;

  await runCommand(copySeedDataCommand);
  await runCommand(restoreBackupCommand);

  // Restore Users
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
