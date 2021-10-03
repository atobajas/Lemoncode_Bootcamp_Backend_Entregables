import crypto from 'crypto';
import { promisify } from 'util';
const ramdomBytes = promisify(crypto.randomBytes);
const pbkdf2 = promisify(crypto.pbkdf2);

const saltLength = 16; // 16 bytes (minimo 8)
export const generateSalt = async (): Promise<string> => {
  const salt = await ramdomBytes(saltLength);
  return salt.toString('hex');
};

const iterations = 100000;
const passwordLength = 64; // 64 bytes = 512 bits.
const digestAlgorithm = 'sha512';
export const hashPassword = async (password: string, salt: string) => {
  const hashedPassword = await pbkdf2(
    password,
    salt,
    iterations,
    passwordLength,
    digestAlgorithm
  );

  return hashedPassword.toString('hex');
};
