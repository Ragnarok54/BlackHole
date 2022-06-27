export class User {
    public userId: string;
    public firstName: string;
    public lastName: string;
    public token: string;
    public phoneNumber: string;
    public picture: string;

    // constructor(JSON: any){
    //     this._firstName = JSON.FirstName;
    //     this._lastName = JSON.LastName;
    //     this._token = JSON.Token;
    //     this._phoneNumber = JSON.PhoneNumber;
    //     this.picture = JSON.picture;
    // }

    constructor(init?: Partial<User>) {
        Object.assign(this, init);
    }

    public name(): string{
        return this.lastName + " " + this.firstName;
    }
}