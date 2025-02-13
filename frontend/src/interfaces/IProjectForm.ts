import { ICustomerForm } from "./ICustomer";
import { IServiceForm } from "./IService";
import { IUserForm } from "./IUser";

export interface IProjectForm {
    name: string;
    startTime: string;
    endTime: string;
    status: string;
    projectManagerId: number;
    userForm: IUserForm | null;
    serviceId: number;
    serviceForm: IServiceForm | null;
    customerId: number;
    customerForm: ICustomerForm | null;
}
