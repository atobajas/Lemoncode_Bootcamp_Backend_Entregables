import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { userRepository } from 'dals';
import { UserSession } from 'common-app/models';
import { envConstants } from 'core/constants';

export const securityApi = Router();

securityApi
  .post('/login', async (req, res, next) => {
    try {
      const { email, password } = req.body;
      const user = await userRepository.getUserByEmailAndPassword(
        email,
        password
      );
      if (user) {
        const userSession: UserSession = {
          id: user._id.toHexString(),
          role: user.role,
        };
        const token = jwt.sign(userSession, envConstants.AUTH_SECRET, {
          expiresIn: '1d',
          algorithm: 'HS256',
        });
        res.send(`Bearer ${token}`);
      } else {
        res.sendStatus(401);
      }
    } catch (error) {
      next(error);
    }
  })
  .post('/logout', async (req, res, next) => {
    try {
      res.sendStatus(200);
    } catch (error) {
      next(error);
    }
  });
