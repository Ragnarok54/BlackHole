import { LoginUser } from "./loginUser";

export class RegisterUser extends LoginUser {
    private firstName: string;
    private lastName: string;

    constructor(firstName: string, lastName: string, phoneNumber: string, password: string){
        super(phoneNumber, password);
        this.firstName = firstName;
        this.lastName = lastName;
    }
}