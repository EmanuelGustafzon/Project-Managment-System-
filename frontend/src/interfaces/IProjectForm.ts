import { ICustomerForm } from "./ICustomer";

export interface IProjectForm {
    name: string;
    startTime: string;
    endTime: string;
    status: string;
    projectManagerId: number;
    serviceId: number;
    customerId: number;
    customerForm: ICustomerForm | null;
}
