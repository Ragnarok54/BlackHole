import { LoginUser } from "./loginUser";

export class RegisterUser extends LoginUser {
    private firstName: string;
    private lastName: string;
    private picture: string;

    constructor(firstName: string, lastName: string, phoneNumber: string, password: string, picture: string){
        super(phoneNumber, password);
        this.firstName = firstName;
        this.lastName = lastName;
        this.picture = picture;
    }
}