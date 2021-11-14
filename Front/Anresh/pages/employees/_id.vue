<template>
  <section>
    <h1 v-if="department">Employees department: {{ department.name }}</h1>
    <employee-table :departmentId="$route.params.id" />
  </section>
</template>

<script>
import employeeTable from '~/components/Employee/EmployeeTable.vue'
export default {
  data() {
    return {
      department: null,
    }
  },
  components: { employeeTable },
  validate({params}) {
    return /^\d+$/.test(params.id)
  },
  async mounted() {
      let { data } = await this.$axios.get("/api/department/simple/" + this.$route.params.id);
      this.department = data;
  }
  
}
</script>

