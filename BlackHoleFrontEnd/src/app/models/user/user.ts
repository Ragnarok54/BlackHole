export class User {
    private _firstName: string;
    private _lastName: string;
    private _token: string;
    private _phoneNumber: string;

    constructor(JSON: any){
        this._firstName = JSON.FirstName;
        this._lastName = JSON.LastName;
        this._token = JSON.Token;
        this._phoneNumber = JSON.PhoneNumber;
    }

    get firstName(){
        return this._firstName;
    }

    get lastName(){
        return this._lastName;
    }

    get name(){
        return this._lastName + " " + this._firstName;
    }

    get phoneNumber(){
        return this._phoneNumber;
    }

    get token(){
        return this._token;
    }
}