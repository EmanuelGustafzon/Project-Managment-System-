
import { useState } from 'react'
import './App.css'
import Navbar from './components/Navbar'
import ProjectForm from './components/ProjectForm'
import ProjectList from './components/ProjectList'

function App() {

  return (
      <div className="mt-0">
          <Navbar createProject={() => document.getElementById('my_modal_1').showModal()!} /> 
          <ProjectList />
          
          <dialog id="my_modal_1" className="modal bg-dark">
              <div className="modal-box bg-zinc-900">
                  <ProjectForm />
                  <div className="modal-action">
                      <form method="dialog">
                          <button className="btn">Close</button>
                      </form>
                  </div>
              </div>
          </dialog>
    </div>
  )
}

export default App
