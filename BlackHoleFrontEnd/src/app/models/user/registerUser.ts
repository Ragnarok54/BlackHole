import { LoginUser } from "./loginUser";

export class RegisterUser extends LoginUser {
    private _firstName: string;
    private _lastName: string;

    constructor(firstName: string, lastName: string, phoneNumber: string, password: string){
        super(phoneNumber, password);
        this._firstName = firstName;
        this._lastName = lastName;
    }

    get firstName(){
        return this._firstName;
    }

    get lastName(){
        return this._lastName;
    }
}