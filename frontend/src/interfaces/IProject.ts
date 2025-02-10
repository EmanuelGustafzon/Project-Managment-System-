import { ICustomer } from "./ICustomer";
import { IService } from "./IService";
import { IUser } from "./IUser";

export interface IProject {
    id: number;
    name: string;
    startTime: string;
    endTime: string;
    status: string;
    totalPrice: number;
    customer: ICustomer;
    service: IService;
    projectManager: IUser;
}