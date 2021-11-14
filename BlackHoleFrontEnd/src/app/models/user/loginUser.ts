export class LoginUser {
    private phoneNumber: string;
    private password: string;

    constructor(phoneNumber: string, password: string){
        this.phoneNumber = phoneNumber;
        this.password = password;
    }
}