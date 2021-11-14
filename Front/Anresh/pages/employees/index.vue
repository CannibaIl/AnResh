<template>
<section>
  <h1>Employees</h1>
  <employee-table :departmentId="''" />


</section>
   
</template>

<script>
import employeeTable from '~/components/Employee/EmployeeTable.vue';
export default {
  components: { employeeTable },
}
</script>

