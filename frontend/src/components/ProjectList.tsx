import { useEffect, useState } from "react";
import { IProject } from "../interfaces/IProject";

const Project = ({ project }: { project: IProject }) => {
    return (
        <>
            <tbody>
                <tr>
                    <td>{project.name}</td>
                    <td>{project.startTime}</td>
                    <td>{project.endTime}</td>
                    <td>{project.status}</td>
                    <td>{project.totalPrice}</td>
                    <td>{`${project.projectManager.firstName} ${project.projectManager.lastName}`}</td>
                    <td>{project.service.name}</td>
                    <td>{project.customer.name}</td>
                </tr>
            </tbody>
        </>
    );
}

const ProjectList = () => {
    const [data, setData] = useState<IProject[]>();

    const url = "https://localhost:7172";
    const endpoint = "api/Project";

    useEffect(() => {
        const getData = async () => {
            try {
                const res = await fetch(`${url}/${endpoint}`);
                const data = await res.json();
                setData(data.data);
                console.log(data.data);
            } catch (error) {
                console.log(error);
            } 
        };
        getData();
    }, []);

    return (
        <div className="overflow-x-auto">
            <table className="table table-xs table-pin-rows table-pin-cols">
                <thead>
                    <tr>
                        <td>Name</td>
                        <td>StartDate</td>
                        <td>EndDate</td>
                        <td>Status</td>
                        <td>TotalPrice</td>
                        <td>ProjectManager</td>
                        <td>Service</td>
                        <td>Customer</td>
                    </tr>
                </thead>
                    {
                    data && data.map((project: IProject) => {
                        return (
                            <Project
                                key={project.id}
                                project={project}
                                />
                            );
                        })
                    }
            </table>
        </div>
    )
}

export default ProjectList;




