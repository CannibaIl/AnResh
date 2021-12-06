import Vue from 'vue'

function getFullName(employee) {
    const firstName = employee.firstName.charAt(0) + '. ';
    const midleName = employee.midleName == null ? '' : employee.midleName.charAt(0)+ '.';
    return `${employee.lastName} ${firstName} ${midleName}`
}
Vue.prototype.$getFullName = getFullName

