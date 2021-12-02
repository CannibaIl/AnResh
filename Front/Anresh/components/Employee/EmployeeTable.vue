<template>
  <section>
    <edit-employee-modal
      ref="editEmployeeModal"
      @row-updated="updateDataTable"
    />
    <delete-employee-modal
      ref="deleteEmployeeModal"
      @row-updated="updateDataTable"
    />

    <b-button
      button
      class="custom-btn primary d-block mt-2 mb-4"
      variant="primary"
      v-if="!hasEmployees && !isBusy && isAdmin"
      @click.prevent="handleCreateEmployee()"
    >
      <fa icon="plus" style="transform: scale(0.8)" /> Add
    </b-button>

    <div v-if="isBusy || hasEmployees" class="big-table-wrap table-responsive">
      <div class="mb-2 d-flex flex-wrap justify-content-between">
        <b-button
          v-if="isAdmin"
          button
          @click.prevent="handleCreateEmployee()"
          class="custom-btn btn primary mr-2 mt-2"
          variant="primary"
          ><fa icon="plus" style="transform: scale(0.8)" /> Add</b-button
        >
        <b-button
          @click.prevent="handleDeleteSelectedEmployees()"
          v-if="selectedEmployees.length && isAdmin"
          class="custom-btn mt-2"
          variant="danger"
          ><fa icon="trash" style="transform: scale(0.8)" /> Delete
          selected</b-button  
        >
      </div>

      <table :busy="isBusy"
        :class="isAdmin ? 'table table-hover table-striped table-borderless big-table big-table-buttons big-table-select-column ' 
                        : 'table table-hover table-striped table-borderless big-table' ">
        <thead>
          <tr>
            <th class="col" v-if="isAdmin"></th>
            <th class="col-1" @click="orderBy('Id')" >Id</th>
            <th class="col-1" @click="orderBy('LastName')">LastName</th>
            <th class="col-1" @click="orderBy('FirstName')">FirstName</th>
            <th class="col-1" @click="orderBy('MiddleName')">MiddleName</th>
            <th class="col-1" @click="orderBy('DepartmentName')">Department</th>
            <th class="col-1" @click="orderBy('Salary')">Salary</th>
            <th class="col-6" @click="orderBy('SkillsCount')">Skills</th>
            <th class="col" v-if="isAdmin"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="employee in employees" :key="employee.id">
            <td v-if="isAdmin">
              <b-form-checkbox
                v-model="selectedEmployees"
                :value="employee.id"
              ></b-form-checkbox>
            </td>
            <td>{{ employee.id }}</td>
            <td>{{ employee.lastName }}</td>
            <td>{{ employee.firstName }}</td>
            <td>{{ employee.middleName }}</td>
            <td>{{ employee.departmentName }}</td>
            <td>{{ employee.salary.toFixed(2) }} $
            </td>
            <td class="employee-skill-td">
              <span class="employee-skill-item" v-for="(skill, i) in employee.skills" :key="i">
                {{ skill.name }}
              </span>
            </td>
            <td v-if="isAdmin">
              <div>
                <button @click.prevent="handleEditEmployee(employee)">
                  <fa class="mr-2 table-ico table-ico-primary" icon="edit" />
                </button>
                <button @click.prevent="handleDeleteEmployee(employee)">
                  <fa class="table-ico table-ico-danger" icon="trash" />
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      
      <div v-if="isBusy" class="text-center text-danger my-2">
        <b-spinner class="align-middle"></b-spinner>
        <strong>Loading...</strong>
      </div>

      <b-pagination
        class="mt-2 custom-pagination"
        v-model="currentPage"
        :total-rows="totalRows"
        :per-page="pageParams.take"
        @page-click="onPageClick"
        >
      </b-pagination>

    </div>
    <span v-else>There are no employees</span>
  </section>
</template>

<script>
import DeleteEmployeeModal from "~/components/Employee/DeleteEmployeeModal.vue";
import EditEmployeeModal from "~/components/Employee/EditEmployeeModal.vue";
export default {
  components: { EditEmployeeModal, DeleteEmployeeModal },
  props: ["departmentId"],

  data() {
    return {
    currentPage: 1,
    totalRows: null,
    isBusy: true,
    isAdmin: false,
    selectedEmployees: [],
    employees: [],
    pageParams: {
      take: 10,
      skip: 0,
      orderBy: 'FirstName',
      ascDesc: 'asc'
    },
  }
},

  computed: {
    hasEmployees() {
      return this.employees.length > 0;
    },
  },

  async mounted() {
    await this.setTotalRows();
    this.employees = await this.getEmployees();
    this.isAdmin = this.$auth.user.role === "Admin";
    this.isBusy = false;
  },

  methods: {
    async getEmployees() {
      const pageQuery = `?Take=${this.pageParams.take}
                          &Skip=${this.pageParams.skip}
                          &OrderBy=${this.pageParams.orderBy}
                          &AscDesc=${this.pageParams.ascDesc}`;
      const id = this.departmentId;
      let { data } =
      id === ""
        ? await this.$axios.get('/api/employee'+ pageQuery)
        : await this.$axios.get('/api/employee/department/' + id + pageQuery);
      return data;
    },

    async setTotalRows() {
      const {data} = await this.$axios.get('/api/employee/totalRows');
      this.totalRows = data;
    },

    async onPageClick(event, page) {
      this.pageParams.skip = (page - 1) * this.pageParams.take;
      this.employees = await this.getEmployees();
    },

    async orderBy(orderBy) {
      if(this.pageParams.orderBy === orderBy) {
        this.pageParams.ascDesc = this.pageParams.ascDesc === 'asc' ? 'desc' : 'asc';
      } else {
        this.pageParams.ascDesc = 'asc';
      }
      this.pageParams.orderBy = orderBy;
      this.pageParams.skip = 0;
      this.currentPage = 1;
      this.employees = await this.getEmployees();
    },

    handleCreateEmployee() {
      this.$refs["editEmployeeModal"].open();
    },

    handleEditEmployee(employee) {
      this.$refs["editEmployeeModal"].open(employee);
    },

    handleDeleteEmployee(employee) {
      this.$refs["deleteEmployeeModal"].open(employee);
    },

    handleDeleteSelectedEmployees() {
      this.$refs["deleteEmployeeModal"].open(this.selectedEmployees);
    },

    async updateDataTable() {
      let response = await this.getEmployees();
      if(response.length === 0) {
        this.pageParams.skip = 0;
        this.currentPage = 1;
        response = await this.getEmployees();
      }
      this.employees = response;
      await this.setTotalRows();
    },
  },
};
</script>


<style lang="scss" scoped>
.employee-skill-td {
  .employee-skill-item::after {
    content: ", ";
    position: relative;
    right: 2px;
  }
  .employee-skill-item:last-child::after {
    content: "";
  }
}
</style>