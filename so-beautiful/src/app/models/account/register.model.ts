import { GenderType } from '../enums/gender-type.enum';

export class Register {
  UserName!: string;
  Password!: string;
  PasswordConfirm!: string;
  Email!: string;
  Surname!: string;
  GivenName!: string;
  Gender!: GenderType;
  DateOfBirth!: Date;
}
