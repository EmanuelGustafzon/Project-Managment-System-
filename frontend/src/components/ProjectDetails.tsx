import { IProject } from "../interfaces/IProject";

const ProjectDetails = ({ project }: { project: IProject }) => {
    return (
        <div className="max-w-xl mx-auto p-6 bg-white shadow-lg rounded-2xl">
            <h2 className="text-2xl font-semibold mb-4">Project Details</h2>
            <div className="space-y-3">
                <p><span className="font-semibold">project Number:</span> {project.id}</p>
                <p><span className="font-semibold">Name:</span> {project.name}</p>
                <p><span className="font-semibold">Start Time:</span> {project.startTime}</p>
                <p><span className="font-semibold">End Time:</span> {project.endTime}</p>
                <p><span className="font-semibold">Status:</span> {project.status}</p>
                <p><span className="font-semibold">Total Price:</span> {project.totalPrice}</p>
            </div>

            <h3 className="text-xl font-semibold mt-4">Customer</h3>
            <div className="space-y-2">
                <p><span className="font-semibold">Name:</span> {project.customer.name}</p>
                <p><span className="font-semibold">Organisation Number:</span> {project.customer.organisationNumber}</p>
            </div>

            <h3 className="text-xl font-semibold mt-4">Service</h3>
            <div className="space-y-2">
                <p><span className="font-semibold">Name:</span> {project.service.name}</p>
                <p><span className="font-semibold">Currency:</span> {project.service.currency}</p>
                <p><span className="font-semibold">Unit:</span> {project.service.unit}</p>
                <p><span className="font-semibold">Price:</span> {project.service.price}</p>
            </div>

            <h3 className="text-xl font-semibold mt-4">Project Manager</h3>
            <div className="space-y-2">
                <p><span className="font-semibold">Name:</span> {project.projectManager.firstName} {project.projectManager.lastName}</p>
                <p><span className="font-semibold">Email:</span> {project.projectManager.email}</p>
            </div>
        </div>
    );
};

export default ProjectDetails;