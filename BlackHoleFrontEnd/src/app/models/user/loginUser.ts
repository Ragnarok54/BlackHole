export class LoginUser {
    private _phoneNumber: string;
    private _password: string;

    constructor(phoneNumber: string, password: string){
        this._phoneNumber = phoneNumber;
        this._password = password;
    }

    get phoneNumber(){
        return this._phoneNumber;
    }
}