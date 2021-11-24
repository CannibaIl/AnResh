<template>
  <section>
    <h1>Departments</h1>

    <department-table />
  </section>
</template>

<script>
import departmentTable from '~/components/Department/DepartmentTable.vue';
export default {
  //middleware: 'auth-admin',
  components: { departmentTable },
};
</script>

<style scoped>
</style>
