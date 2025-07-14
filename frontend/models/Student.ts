export class Student {
    id?: number;
    name: string;
    email: string;
    ra: string;
    cpf: string;
    createdAt?: Date;
    updatedAt?: Date;

    constructor(name = "", email = "", ra = "", cpf = "") {
        this.name = name;
        this.email = email;
        this.ra = ra;
        this.cpf = cpf;
    }
}
