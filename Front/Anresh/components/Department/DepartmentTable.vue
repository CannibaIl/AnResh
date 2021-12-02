<template>
  <section>
    <edit-department-modal
      ref="editDepartmentModal"
      @row-updated="updateDataTable"
    />
    <delete-department-modal
      ref="deleteDepartmentModal"
      :departments="departments"
      @row-updated="updateDataTable"
    />

    <b-button
      class="custom-btn d-block mt-2 mb-4"
      variant="primary"
      v-if="!hasDepartments && !isBusy && isAdmin"
      @click.prevent="handleCreateDepartment()"
      ><fa icon="plus" style="transform: scale(0.8)" /> Add
    </b-button>


<div v-if="isBusy || hasDepartments" class="big-table-wrap table-responsive">
      <b-button
       v-if="isAdmin"
        @click.prevent="handleCreateDepartment()"
        class="custom-btn mt-2"
        variant="primary"
        ><fa icon="plus" style="transform: scale(0.8)" /> Add
        </b-button>

      <table :busy="isBusy"
        :class="isAdmin ? 'table table-hover table-striped table-borderless big-table big-table-buttons' 
                        : 'table table-hover table-striped table-borderless big-table' ">
        <thead>
          <tr>
            <th class="col-1" @click="orderBy('Id')" >Id</th>
            <th class="col-7" @click="orderBy('Name')">Name</th>
            <th class="col-2" @click="orderBy('EmployeeCount')">Employees Count</th>
            <th class="col-2" @click="orderBy('AverageSalary')">Average salary</th>
            <th class="col" v-if="isAdmin"></th>
          </tr>
        </thead>
        <tbody>
          <tr @click="goToEmployees(department)" v-for="department in departments" :key="department.id">
            <td>{{ department.id }}</td>
            <td>{{ department.name }}</td>
            <td>{{ department.employeeCount }}</td>
            <td>{{ department.averageSalary.toFixed(2) }} $</td>
            <td v-if="isAdmin">
              <div>
                <button @click.prevent="handleEditDepartment($event, department)">
              <fa class="mr-2 table-ico table-ico-primary" icon="edit" />
              </button>
              <button @click.prevent="handleDeleteDepartment($event, department)">
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
    <span v-else>There are no departments</span>
  </section>
</template>

<script>
import EditDepartmentModal from "~/components/Department/EditDepartmentModal.vue";
import DeleteDepartmentModal from "~/components/Department/DeleteDepartmentModal.vue";
export default {
  components: { EditDepartmentModal, DeleteDepartmentModal },

  data: () => ({
    isAdmin: false,
    isBusy: true,
    currentPage: 1,
    totalRows: null,
    departments: [],
    pageParams: {
      take: 10,
      skip: 0,
      orderBy: 'EmployeeCount',
      ascDesc: 'desc'
    },
  }),

  computed: {
    hasDepartments() {
      return this.departments.length > 0;
    },
  },

  async mounted() {
    await this.setTotalRows();
    this.departments = await this.getDepartments(); 
    this.isAdmin =  this.$auth.user.role === "Admin";
    this.isBusy = false;
  },

  methods: {
    async getDepartments() {
      let pageQuery = `?Take=${this.pageParams.take}
                       &Skip=${this.pageParams.skip}
                       &OrderBy=${this.pageParams.orderBy}
                       &AscDesc=${this.pageParams.ascDesc}`;
                       
      let { data } = await this.$axios.get('/api/department' + pageQuery);
      return data
    },

    async setTotalRows() {
      const {data} = await this.$axios.get('/api/department/totalRows');
      this.totalRows = data;
    },

    async onPageClick(event, page) {
      this.pageParams.skip = (page - 1) * this.pageParams.take;
      this.departments = await this.getDepartments();
      await this.setTotalRows();
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
      this.departments = await this.getDepartments();
      await this.setTotalRows();
    },

    goToEmployees(department) {
      if(department.employeeCount > 0){
         this.$router.push({ name: 'employees-id', params: { id: department.id } });
      }
    },

    handleCreateDepartment() {
      this.$refs["editDepartmentModal"].open();
    },

    handleEditDepartment(event, department) {
      event.stopImmediatePropagation();
      this.$refs["editDepartmentModal"].open(department);
    },

    handleDeleteDepartment(event, department) {
      event.stopImmediatePropagation();
      this.$refs["deleteDepartmentModal"].open(department);
    },

    addDepartment(data) {
      this.departments.push(data);
    },

    async updateDataTable() {
      let response = await this.getDepartments();
      if(response.length === 0) {
        this.pageParams.skip = 0;
        this.currentPage = 1;
        response = await this.getDepartments();
      }
      this.departments = response;
      await this.setTotalRows();
    },
  },
};
</script>
