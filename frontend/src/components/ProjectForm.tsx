import { useState } from "react";
import useFetch from "../hooks/useFetch";
import AddUser from "./AddUser";
import AddCustomer from "./AddCustomer";
import AddProjectInfo from "./AddProjectInfo";


const ProjectForm = () => {
    //const [projectForm, setProjectForm] = useState<IProject>()
    const [step, setStep] = useState(0);
    //user
    const [selectedUser, setSelectedUser] = useState(0);
    // customer
    const [selectedCustomer, setSelectedCustomer] = useState(0);
    const [customerForm, setCustomerForm] = useState({ name: '', organisationNumber: '' });

    const [selectedStatus, setSelectedStatus] = useState("");
    console.log(selectedUser, selectedStatus, selectedCustomer,customerForm);

    const handleSelectUserChange = (user: { id: string }) => {
        setSelectedUser(Number(user.id)); 
    };
    const handleSelectCustomerChange = (customer: { id: string }) => {
        setSelectedCustomer(Number(customer.id));
    };
    const handleSelectStatusChange = (status: string) => {
        setSelectedStatus(status);
    };
    const handleCustomerFormChange = (createdCustomer: { name: string; organisationNumber: string }) => {
        setCustomerForm(createdCustomer);
    }

    return (
        <div className="bg-sky-100 p-3">
            {step === 0 && <AddProjectInfo onStatusChange={handleSelectStatusChange} />}
            {step === 1 && <AddUser onUserChange={handleSelectUserChange} />}
            {step === 2 && <AddCustomer onChooseCustomerChange={handleSelectCustomerChange} customer={customerForm} onCreateCustomer={handleCustomerFormChange} />}
            <div className="flex flex-wrap justify-center items-center gap-2 m-5">
                <button className="btn" onClick={() => setStep(lastStep => lastStep - 1)}>Prev</button>
                <button className="btn"  onClick={() => setStep(lastStep => lastStep + 1)}>next</button>
            </div>
            
        </div>
    )
}

export default ProjectForm;