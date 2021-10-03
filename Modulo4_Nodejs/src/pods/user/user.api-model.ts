import { Role } from 'common-app/models';

export interface User {
  email: string;
  password: string;
  role: Role;
}
