// script.js

const apiUrl = 'https://localhost:44358/api'; // Replace with your API base URL

function fetchEmployees() {
    fetch(`${apiUrl}/employees`)
        .then(response => response.json())
        .then(employees => {
            const employeeList = document.getElementById('employeeList');
            employeeList.innerHTML = '';
            employees.forEach(employee => {
                const li = document.createElement('li');
                li.textContent = `${employee.name} - ${employee.department}`;
                employeeList.appendChild(li);
            });
        })
        .catch(error => console.error('Error fetching employees:', error));
}

function submitEmployeeForm(event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);

    const employee = {
        name: formData.get('name'),
        department: formData.get('department')
    };

    saveEmployee(employee);
}

function saveEmployee(employee) {
    fetch(`${apiUrl}/employees`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(employee),
    })
        .then(response => {
            if (response.ok) {
                console.log('Employee saved successfully');
                // Optionally update UI or show success message
                fetchEmployees(); // Refresh employee list
            } else {
                throw new Error('Failed to save employee');
            }
        })
        .catch(error => console.error('Error saving employee:', error));
}

document.getElementById('employeeForm').addEventListener('submit', submitEmployeeForm);
