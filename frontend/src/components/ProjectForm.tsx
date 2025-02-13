import { useState } from "react";
import AddUser from "./AddUser";
import AddCustomer from "./AddCustomer";
import AddProjectInfo from "./AddProjectInfo";
import { IProjectForm } from "../interfaces/IProjectForm";
import AddService from "./AddService";
import useSendData from "../hooks/useSendData";


const ProjectForm = () => {
    const { makeRequest, validationError, error, response } = useSendData("api/Project")

    const [projectForm, setProjectForm] = useState<IProjectForm>({
        name: "",
        startTime: "",
        endTime: "",
        status: "",
        projectManagerId: 0,
        serviceId: 1,
        serviceForm: {
            name: "",
            currency: "",
            unit: "",
            price: 0,
        },
        userForm: {
            firstName: "",
            lastName: "",
            email: ""
        },
        customerId: 0,
        customerForm:
        {
            name: "",
            organisationNumber: ""
        }
    });

    
    // form steps
    const totalSteps = 5;
    const [step, setStep] = useState(0);
    // step 1 add info about project
    const handleProjectInfoChange = (projectForm : { name: string, startTime: string, endTime: string, status: string }) => {
        setProjectForm(prev => ({
            ...prev,
            name: projectForm.name,
            startTime: projectForm.startTime,
            endTime: projectForm.endTime,
            status: projectForm.status,
        }));
    };
    // stetp 2 add user
    const handleSelectUserChange = (user: { id: string }) => {
        setProjectForm(prev => ({
            ...prev,
            projectManagerId: Number(user.id),
            userForm: {
                firstName: "", lastName: "", email: ""
            }
        }));
    };
    const handleCreateUserChange = (createdUser: { firstName: string; lastName: string, email: string }) => {
        setProjectForm(prev => ({
            ...prev,
            userForm: createdUser
        }));
    };
    // step 3 add customer
    const handleSelectCustomerChange = (customer: { id: string }) => {
        setProjectForm(prev => ({
            ...prev,
            customerId: Number(customer.id),
            customerForm: {
                name: "", organisationNumber: ""
            }
        }));
    };
    const handleCreateCustomerChange = (createdCustomer: { name: string; organisationNumber: string }) => {
        setProjectForm(prev => ({
            ...prev,
            customerForm: createdCustomer,
            customerId: 0
        }));
    }
    // step 4 add service
    const handleSelectServiceChange = (service: { id: string }) => {
        setProjectForm(prev => ({
            ...prev,
            serviceId: Number(service.id),
            serviceForm: {
                name: "", currency: "", unit: "", price: 0
            }
        }));
    };
    const handleCreateServiceChange = (createdService: { name: string, currency: string, unit: string, price: number }) => {
        setProjectForm(prev => ({
            ...prev,
            serviceForm: createdService,
            serviceId: 0
        }));
    }
    // step 5 create project action
    const createProject = () => {
        if (projectForm.customerId > 0) projectForm.customerForm = null;
        makeRequest(projectForm);
    }

    return (
        <div className="bg-sky-100 p-3">
            {step === 0 && <AddProjectInfo onProjectInfoChange={handleProjectInfoChange} projectForm={projectForm} />}
            {step === 1 && <AddUser onChooseUserChange={handleSelectUserChange} onCreateUser={handleCreateUserChange} user={projectForm.userForm!} userId={projectForm.projectManagerId} />}
            {step === 2 && <AddCustomer onChooseCustomerChange={handleSelectCustomerChange} onCreateCustomer={handleCreateCustomerChange} customer={projectForm.customerForm!} customerId={projectForm.customerId} />}
            {step === 3 && <AddService onChooseServiceChange={handleSelectServiceChange} onCreateService={handleCreateServiceChange} service={projectForm.serviceForm!} serviceId={projectForm.serviceId} />}
            {step === 4 &&
                <div className="text-center">
                    <button className="btn btn-wide btn-neutral" onClick={createProject}>Create Project</button>
                    <ul>
                        {validationError &&
                            <div>
                                <h2 className="font-extrabold text-lg">Sorry there was some errors please recheck the form</h2>
                                {Object.entries(validationError).map(([key, value]) => (
                                    <li>
                                        <p className="font-bold"> Field: {key}</p>
                                        <p className="text-red-400"> Error: {value[0]}</p>
                                    </li>
                                ))}
                            </div>
                        }
                    </ul>
                    {error && <p className="text-red-400">{error}</p>}
                    {error && <p className="text-red-400">{error}</p>}
                    {response && <p>{response}</p> }
                </div>}
            <div className="flex flex-wrap justify-center items-center gap-2 m-5">
                <button className="btn" onClick={() => setStep(lastStep => (lastStep - 1 + 5) % 5)}>Prev</button>
                {step <= totalSteps }
                <button className="btn"  onClick={() => setStep(lastStep => (lastStep + 1) % 5)}>next</button>
            </div>
            
        </div>
    )
}

export default ProjectForm;